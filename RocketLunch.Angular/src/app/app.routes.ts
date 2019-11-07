import { LoginComponent } from './components/login/login.component';
import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./components/home/home.component";
import { TooManyRequestsComponent } from "./components/too-many-requests/too-many-requests.component";

export const routes: Routes = [
	{
		path: '',
		children: [
			{ path: '', component: HomeComponent },  // default page
			{ path: 'home', component: HomeComponent }, 
			{ path: 'login', component: LoginComponent }, 
			{ path: 'too-many', component: TooManyRequestsComponent },
		],
	},
	{ path: '**', component: HomeComponent }
];

export const routing = RouterModule.forRoot(routes);