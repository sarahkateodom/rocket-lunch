import { ProfileComponent } from './components/profile/profile.component';
import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./components/home/home.component";
import { TooManyRequestsComponent } from "./components/too-many-requests/too-many-requests.component";
import { WelcomeComponent } from './components/welcome/welcome.component';

export const routes: Routes = [
	{
		path: '',
		children: [
			{ path: '', component: WelcomeComponent },  // default page
			{ path: 'welcome', component: WelcomeComponent }, 
			{ path: 'home', component: HomeComponent }, 
			{ path: 'too-many', component: TooManyRequestsComponent },
			{ path: 'profile/:id', component: ProfileComponent },
		],
	},
	{ path: '**', component: WelcomeComponent }
];

export const routing = RouterModule.forRoot(routes);