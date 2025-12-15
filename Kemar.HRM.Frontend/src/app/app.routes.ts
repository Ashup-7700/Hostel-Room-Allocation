import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { DashboardComponent } from './components/dashboard/dashboard';

export const routes: Routes = [

  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent },

  { path: 'students', loadComponent: () =>
      import('./components/student/student').then(m => m.Student)
  },

  { path: 'rooms', loadComponent: () =>
      import('./components/room/room').then(m => m.Room)
  },

  { path: 'allocation', loadComponent: () =>
      import('./components/room-allocation/room-allocation').then(m => m.RoomAllocationComponent)
  },

  { path: 'payments', loadComponent: () =>
      import('./components/payment/payment').then(m => m.Payment)
  },

  { path: 'fee-structure', loadComponent: () =>
      import('./components/fee-structure/fee-structure').then(m => m.FeeStructure)
  },

  { path: '**', redirectTo: 'login' }
];
