import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmModalService } from '../_services/confirm-modal.service';


@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent> {

  constructor(private confirmModalService: ConfirmModalService) { }

  canDeactivate(component: MemberEditComponent): Observable<boolean> | boolean {
    if (component.editForm.dirty) {
      return this.confirmModalService.confirm();
    }

    return true;
  }
}
