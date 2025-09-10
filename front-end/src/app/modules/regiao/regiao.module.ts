import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { RegiaoComponent } from './regiao.component';
import { RegiaoFormComponent } from './regiao-form/regiao-form.component'
import { RegiaoRoutingModule } from './regiao.routing';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ComponentsModule,
    RegiaoRoutingModule
  ],
  declarations: [RegiaoComponent, RegiaoFormComponent],
  exports: [RegiaoComponent]
})
export class RegiaoModule { }
