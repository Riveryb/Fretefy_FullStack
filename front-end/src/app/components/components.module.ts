import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CidadeSelectComponent } from './cidade-select/cidade-select.component';

@NgModule({
  declarations: [CidadeSelectComponent],
  imports: [CommonModule, ReactiveFormsModule],
  exports: [CidadeSelectComponent]
})
export class ComponentsModule {}
