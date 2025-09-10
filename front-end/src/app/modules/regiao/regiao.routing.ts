import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegiaoComponent } from './regiao.component';
import { RegiaoFormComponent } from './regiao-form/regiao-form.component'

const routes: Routes = [
  { path: '', component: RegiaoComponent },
  { path: 'novo', component: RegiaoFormComponent },
  { path: ':id/editar', component: RegiaoFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegiaoRoutingModule { }
