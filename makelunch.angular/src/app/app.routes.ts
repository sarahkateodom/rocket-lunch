import { Routes, RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { HomeComponent } from "./components/home/home.component";
import { TooManyRequestsComponent } from "./components/too-many-requests/too-many-requests.component";

export const routes: Routes = [
	{
		path: '',
		children: [
			{ path: '', component: HomeComponent },  // default page
			{ path: 'home', component: HomeComponent }, 
			{ path: 'too-many', component: TooManyRequestsComponent },
		],
	},
	{ path: '**', component: HomeComponent }
];

export const routing = RouterModule.forRoot(routes);