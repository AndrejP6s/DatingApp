import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { MessagesComponent } from './messages/messages.component';
import { ConfirmDialogComponent } from './modals/confirm-dialog/confirm-dialog.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { NavComponent } from './nav/nav.component';
import { RegisterComponent } from './register/register.component';
import { SharedModule } from './shared.module';
import { HasRoleDirective } from './_directives/has-role.directive';
import { DateInputComponent } from './_forms/date-input/date-input.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ErrorInterceptorProvider } from './_interceptors/error.interceptor';
import { JwtInterceptorProvider } from './_interceptors/jwt.interceptor';
import { LoadingInterceptorProvider } from './_interceptors/loading.interceptor';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { AuthService } from './_services/auth.service';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    ListsComponent,
    MessagesComponent,
    NotFoundComponent,
    ServerErrorComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DateInputComponent,
    MemberMessagesComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
    ConfirmDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    SharedModule
  ],
  providers: [
    AuthService,
    AuthGuard,
    PreventUnsavedChanges,
    ErrorInterceptorProvider,
    JwtInterceptorProvider,
    LoadingInterceptorProvider,
    MemberDetailResolver,
    MemberEditResolver,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
