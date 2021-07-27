using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Extensions;
using DatingApp.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _presenceTracker;

        public MessageHub(IUnitOfWork unitOfWork, IMapper mapper,
            IHubContext<PresenceHub> presenceHub, PresenceTracker presenceTracker)
        {
            _unitOfWork = unitOfWork;
            _presenceTracker = presenceTracker;
            _presenceHub = presenceHub;
            _mapper = mapper;

        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(httpContext.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messageThread = await _unitOfWork.MessageRepository.GetMessageThread(httpContext.User.GetUsername(), otherUser);
            
            if (_unitOfWork.HasChanges()) await _unitOfWork.CompleteAsync();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messageThread);

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername) throw new HubException("You cannot send message to yourself");

            var recipient = await _unitOfWork.UserRepository.GetUserByUsername(createMessageDto.RecipientUsername);

            if (recipient == null) throw new HubException($"User {createMessageDto.RecipientUsername} not found");

            var sender = await _unitOfWork.UserRepository.GetUserByUsername(username);
            var message = new Message
            {
                Sender = sender,
                SenderUsername = sender.UserName,
                Recipient = recipient,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            if (group.Connections.Any(x => x.Username == message.RecipientUsername))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connectionIds = await _presenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connectionIds != null)
                {
                    await _presenceHub.Clients.Clients(connectionIds).SendAsync("NewMessageReceived", new
                    {
                        username = sender.UserName,
                        knownAs = sender.KnownAs
                    });
                }
            }

            _unitOfWork.MessageRepository.AddMessage(message);
            if (await _unitOfWork.CompleteAsync())
            {
                var messageDto = _mapper.Map<MessageDto>(message);
                await Clients.Group(groupName).SendAsync("NewMessage", messageDto);
            }

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveConnectionFromGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);

            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(string caller, string otherUser)
        {
            var usernameComparator = string.CompareOrdinal(caller, otherUser) < 0;
            return usernameComparator ? $"{caller}-{otherUser}" : $"{otherUser}-{caller}";
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());
            group.Connections.Add(connection);

            if (await _unitOfWork.CompleteAsync()) return group;

            throw new HubException($"Failed to add connection to {groupName} group");
        }

        private async Task<Group> RemoveConnectionFromGroup()
        {
            var group = await _unitOfWork.MessageRepository.GetGroupForConnetion(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);

            if (await _unitOfWork.CompleteAsync()) return group;

            throw new HubException($"Failed to remove connection from {group.Name} group");
        }
    }
}