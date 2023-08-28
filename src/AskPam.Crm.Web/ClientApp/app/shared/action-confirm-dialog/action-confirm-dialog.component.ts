import {
    ChangeDetectionStrategy,
    Component,
    HostListener,
    Inject
  } from '@angular/core';
  import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
  import { Action, Store } from "@ngrx/store";
  import { AppState } from '../../redux/index';

  @Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-action-confirm-dialog',
    templateUrl: './action-confirm-dialog.component.html',
    styleUrls: ['./action-confirm-dialog.component.scss']
  })
  export class ActionConfirmDialogComponent {
  
    constructor(
      @Inject(MAT_DIALOG_DATA) public data: {
        cancel?: Action,
        action: Action,
        go?: Action,
        text: string,
        title: string
      },
      private mdDialogRef: MatDialogRef<ActionConfirmDialogComponent>,
      private store: Store<AppState>
    ) { }
  
    public cancel() {
      if (this.data.cancel !== undefined){
        this.store.dispatch(this.data.cancel);
      }
      this.close();
    }
  
    public close() {
      this.mdDialogRef.close();
    }
  
    public action() {
      this.store.dispatch(this.data.action);
      if (this.data.go !== undefined) {
        this.store.dispatch(this.data.go);
      }
      this.close();
    }
  
    @HostListener("keydown.esc")
    public onEsc() {
      this.close();
    }
  
  }