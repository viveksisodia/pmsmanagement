import { Component, ElementRef, effect, forwardRef, input, output, signal, viewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { formatDate, parseDisplayDate } from '../core/format';

/** A modal dialog built on the native <dialog> element. */
@Component({
  selector: 'app-modal',
  template: `
    <dialog #dlg class="modal" [class.wide]="wide()" (close)="closed.emit()" (cancel)="$event.preventDefault(); closed.emit()">
      <header><h2>{{ heading() }}</h2><button type="button" class="icon" aria-label="Close" (click)="closed.emit()">×</button></header>
      <div class="body"><ng-content /></div>
      <footer><ng-content select="[actions]" /></footer>
    </dialog>`
})
export class Modal {
  heading = input.required<string>();
  open = input(false);
  wide = input(false);
  closed = output<void>();
  private dlg = viewChild.required<ElementRef<HTMLDialogElement>>('dlg');
  constructor() {
    effect(() => {
      const d = this.dlg().nativeElement;
      if (this.open() && !d.open) d.showModal();
      if (!this.open() && d.open) d.close();
    });
  }
}

/** Text input for dates in dd/mm/yyyy; the form value is yyyy-mm-dd (or null). */
@Component({
  selector: 'app-date-input',
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => DateInput), multi: true }],
  template: `<input [id]="inputId()" type="text" inputmode="numeric" placeholder="dd/mm/yyyy" autocomplete="off"
                    [value]="text()" [class.invalid]="bad()" [disabled]="disabled()"
                    (input)="onInput($any($event.target).value)" (blur)="onBlur()" />
             @if (bad()) { <small class="err">Use dd/mm/yyyy</small> }`
})
export class DateInput implements ControlValueAccessor {
  inputId = input<string>('');
  text = signal('');
  bad = signal(false);
  disabled = signal(false);
  private onChange: (v: string | null) => void = () => {};
  private onTouched: () => void = () => {};
  writeValue(v: string | null): void { this.text.set(v ? formatDate(v) : ''); this.bad.set(false); }
  registerOnChange(fn: (v: string | null) => void): void { this.onChange = fn; }
  registerOnTouched(fn: () => void): void { this.onTouched = fn; }
  setDisabledState(d: boolean): void { this.disabled.set(d); }
  onInput(t: string): void {
    this.text.set(t);
    const v = parseDisplayDate(t);
    this.bad.set(v === undefined && t.trim().length >= 8);
    if (v !== undefined) this.onChange(v);
  }
  onBlur(): void {
    const v = parseDisplayDate(this.text());
    this.bad.set(v === undefined);
    if (v === undefined) this.onChange(null);
    this.onTouched();
  }
}

/** Status pill for unit availability and other states. */
@Component({
  selector: 'app-pill',
  template: `<span class="pill" [attr.data-k]="value()">{{ value() }}</span>`
})
export class Pill { value = input.required<string>(); }
