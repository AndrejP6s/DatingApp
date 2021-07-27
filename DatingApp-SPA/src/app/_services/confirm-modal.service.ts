import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable, Subscriber } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmModalService {
  bsModalRef: BsModalRef;

  constructor(private bsModalService: BsModalService) { }

  confirm(title = 'Confirmation', message = 'Are you sure you want to continue?', okBtnText = 'Ok', closeBtnText = 'Cancel') {
    const initialState = {
      title,
      message,
      okBtnText,
      closeBtnText
    }

    this.bsModalRef = this.bsModalService.show(ConfirmDialogComponent, { initialState });

    return new Observable<boolean>(this.resultSubscriber);
  }

  private resultSubscriber = (observer: Subscriber<boolean>) => {
    const subscription = this.bsModalService.onHidden.subscribe(() => {
      observer.next(this.bsModalRef.content.result);
      observer.complete()
    });

    return {
      unsubscribe() {
        subscription.unsubscribe();
      }
    }
  }
}
