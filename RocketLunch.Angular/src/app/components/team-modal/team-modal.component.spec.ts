import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { User } from '../../models/user';
import { of, throwError } from 'rxjs';
import { LunchLadyService } from '../../services/lunch-lady.service';
import { ModalComponent } from '../modal/modal.component';
import { FormsModule } from '@angular/forms';
import { TeamModalComponent } from './team-modal.component';
import { Team } from 'src/app/models/team';

describe('TeamModalComponent', () => {
    let component: TeamModalComponent;
    let fixture: ComponentFixture<TeamModalComponent>;
    let mockLunchService: any;
    let users = [
        { id: 1, name: "Po Labare", nopes: ['fsad', ';lkj'] } as User,
        { id: 2, name: "Pam Dabare", nopes: ['fsad', ';lkj'] } as User,
    ] as User[];
    let team = { id: 1, name: 'Team', zip: '38655' } as Team;

    TestBed.overrideTemplate(
        TeamModalComponent,
        "<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>");

    beforeEach(async(() => {
        mockLunchService = jasmine.createSpyObj('LunchLadyService', ['getTeamUsers', 'createTeam', 'addUserToTeam', 'removeUserFromTeam'])
        mockLunchService.getTeamUsers.and.returnValue(of(users));
        mockLunchService.createTeam.and.returnValue(of(team));
        mockLunchService.removeUserFromTeam.and.returnValue(of(true));
        TestBed.configureTestingModule({
            declarations: [TeamModalComponent, ModalComponent],
            providers: [
                { provide: LunchLadyService, useValue: mockLunchService },
            ],
            imports: [FormsModule]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(TeamModalComponent);
        component = fixture.componentInstance;
        spyOn(component.modal, "show");
        spyOn(component.modal, "hide");
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('show', () => {
        it('should clear errorMessage', () => {
            // Protec 
            component.errorMessage = 'you should not see me';

            // Attac
            component.show();

            // Eat frog for snac
            expect(component.errorMessage).toBeFalsy();
        });
        it('should set selectedTeam with user.zip if team is not set', () => {
            // Protec 
            component.existingTeam = undefined;
            component.user = {
                zip: '38655'
            } as User;

            // Attac
            component.show();

            // Eat frog for snac
            expect(component.selectedTeam).toEqual({ zip: '38655' } as Team);
        });
        it('should set selectedTeam with empty zip if team and user is not set', () => {
            // Protec 
            component.existingTeam = undefined;
            component.user = undefined;

            // Attac
            component.show();

            // Eat frog for snac
            expect(component.selectedTeam).toEqual({ zip: '' } as Team);
        });
        it('should set selectedTeam to this.team if provided', () => {
            // Protec 
            component.existingTeam = {
                id: 6,
                name: 'The Test Team',
                zip: '38655'
            } as Team;

            // Attac
            component.show();

            // Eat frog for snac
            expect(component.selectedTeam.id).toEqual(component.existingTeam.id);
            expect(component.selectedTeam.name).toEqual(component.existingTeam.name);
            expect(component.selectedTeam.zip).toEqual(component.existingTeam.zip);
        });
        it('should call getTeamUsers if this.team is provided', () => {
            // Protec 
            component.existingTeam = {
                id: 6,
                name: 'The Test Team',
                zip: '38655'
            } as Team;

            // Attac
            component.show();

            // Eat frog for snac
            expect(mockLunchService.getTeamUsers).toHaveBeenCalledWith(component.existingTeam.id);
        });
        it('should set this.users if this.team is provided', () => {
            // Protec 
            component.existingTeam = {
                id: 6,
                name: 'The Test Team',
                zip: '38655'
            } as Team;

            // Attac
            component.show();

            // Eat frog for snac
            expect(component.users).toEqual(users);
        });

        it('should show modal', () => {
            // Protec 

            // Attac
            component.show();

            // Eat frog for snac
            expect(component.modal.show).toHaveBeenCalled();
        });
    });

    describe('hide', () => {
        it('should hide modal', () => {
            // Protec 

            // Attac
            component.hide();

            // Eat frog for snac
            expect(component.modal.hide).toHaveBeenCalled();
        });
    });

    describe('filterUsers', () => {
        it('should return a list of users excluding this.user', () => {
            // Protec 
            component.user = users[0];
            component.users = users;

            // Attac
            let result = component.filterUsers();

            // Eat frog for snac
            expect(result).toContain(users[1]);
            expect(result).not.toContain(users[0]);
        });
    });

    describe('createTeam', () => {
        it('should not call createTeam if this.selectedTeam is undefined', () => {
            // Protec 
            component.user = {
                id: 1,
                teams: [],
            } as User;
            component.selectedTeam = undefined;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(mockLunchService.createTeam).not.toHaveBeenCalled();
        });

        it('should not call createTeam if this.selectedTeam name is not valid', () => {
            // Protec 
            component.user = {
                id: 1,
                teams: [],
            } as User;
            component.selectedTeam = {
                zip: '38655',
                name: undefined
            } as Team;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(mockLunchService.createTeam).not.toHaveBeenCalled();
        });

        it('should not call createTeam if this.selectedTeam zip is not valid', () => {
            // Protec 
            component.user = {
                id: 1,
                teams: [],
            } as User;
            component.selectedTeam = {
                zip: undefined,
                name: 'pony macaroni'
            } as Team;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(mockLunchService.createTeam).not.toHaveBeenCalled();
        });

        it('should not call createTeam if this.user is not set', () => {
            // Protec 
            component.user = undefined;
            component.selectedTeam = {
                zip: '38655',
                name: 'pony macaroni'
            } as Team;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(mockLunchService.createTeam).not.toHaveBeenCalled();
        });

        it('should call createTeam if this.selectedTeam and this.user is valid', () => {
            // Protec 
            component.selectedTeam = {
                zip: '38655',
                name: 'pony macaroni'
            } as Team;
            component.user = {
                id: 1,
                teams: [],
            } as User;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(mockLunchService.createTeam).toHaveBeenCalledWith(component.user.id, component.selectedTeam);
        });

        it('should add created team to this.user.teams', () => {
            // Protec 
            component.selectedTeam = {
                zip: '38655',
                name: 'pony macaroni'
            } as Team;
            component.user = {
                id: 1,
                teams: [],
            } as User;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(component.user.teams).toContain(team);
        });

        it('should hide modal', () => {
            // Protec 
            component.selectedTeam = {
                zip: '38655',
                name: 'pony macaroni'
            } as Team;
            component.user = {
                id: 1,
                teams: [],
            } as User;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(component.modal.hide).toHaveBeenCalled();
        });

        it('should set the error message when createTeam returns an error', () => {
            // Protec 
            let errorMessage = 'halp';
            mockLunchService.createTeam.and.returnValue(throwError({ error: errorMessage }));
            component.selectedTeam = {
                zip: '38655',
                name: 'pony macaroni'
            } as Team;
            component.user = {
                id: 1,
                teams: [],
            } as User;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(component.errorMessage).toEqual(errorMessage);
        });

        it('should set the error message to empty when createTeam returns a success', () => {
            // Protec 
            component.errorMessage = 'halp';
            component.selectedTeam = {
                zip: '38655',
                name: 'pony macaroni'
            } as Team;
            component.user = {
                id: 1,
                teams: [],
            } as User;

            // Attac
            component.createTeam();

            // Eat frog for snac
            expect(component.errorMessage).toEqual('');
        });
    });

    describe('addUser', () => {
        it('should not call lunchService.addUserToTeam when selectedTeam is not valid', () => {
            // Protec 
            component.selectedTeam = undefined;
            component.userToAddEmail = 'asdfasdf';

            // Attac
            component.addUser();

            // Eat frog for snac
            expect(mockLunchService.addUserToTeam).not.toHaveBeenCalled();
        });

        it('should not call lunchService.addUserToTeam when userToAddEmail is not valid', () => {
            // Protec 
            component.selectedTeam = {
                id: 9
            } as Team;
            component.userToAddEmail = undefined;

            // Attac
            component.addUser();

            // Eat frog for snac
            expect(mockLunchService.addUserToTeam).not.toHaveBeenCalled();
        });

        it('should call lunchService.addUserToTeam', () => {
            // Protec 
            let userToAdd = {
                id: 7,
                name: 'bobson dugnutt',
                email: 'bdugs@basebollusa.org',
                photoUrl: 'google.com',
                nopes: [],
                zip: '38655',
                teams: []
            } as User;

            component.selectedTeam = {
                id: 9
            } as Team;
            component.userToAddEmail = userToAdd.email;
            mockLunchService.addUserToTeam.and.returnValue(of(userToAdd));

            // Attac
            component.addUser();

            // Eat frog for snac
            expect(mockLunchService.addUserToTeam).toHaveBeenCalledWith(userToAdd.email, component.selectedTeam.id);
        });

        it('should add user to this.users', () => {
            // Protec 
            let userToAdd = {
                id: 7,
                name: 'bobson dugnutt',
                email: 'bdugs@basebollusa.org',
                photoUrl: 'google.com',
                nopes: [],
                zip: '38655',
                teams: []
            } as User;

            component.selectedTeam = {
                id: 9
            } as Team;
            component.userToAddEmail = userToAdd.email;
            mockLunchService.addUserToTeam.and.returnValue(of(userToAdd));

            // Attac
            component.addUser();

            // Eat frog for snac
            expect(component.users).toContain(userToAdd);
        });

        it('should reset userToAddEmail', () => {
            // Protec 
            let userToAdd = {
                id: 7,
                name: 'bobson dugnutt',
                email: 'bdugs@basebollusa.org',
                photoUrl: 'google.com',
                nopes: [],
                zip: '38655',
                teams: []
            } as User;

            component.selectedTeam = {
                id: 9
            } as Team;
            component.userToAddEmail = userToAdd.email;
            mockLunchService.addUserToTeam.and.returnValue(of(userToAdd));

            // Attac
            component.addUser();

            // Eat frog for snac
            expect(component.userToAddEmail).toBeFalsy();
        });

        it('should set errorMessage if addUserToTeam throws error', () => {
            // Protec 
            component.errorMessage = '';
            let errorMessage = 'halp';
            mockLunchService.addUserToTeam.and.returnValue(throwError({ error: errorMessage }));

            component.selectedTeam = {
                id: 9
            } as Team;
            component.userToAddEmail = 'bdugs@basebollusa.org';

            // Attac
            component.addUser();

            // Eat frog for snac
            expect(component.errorMessage).toEqual(errorMessage);
        });
    });

    describe('removeUserFromTeam', () => {
        it('should call lunchService.removeUserFromTeam', () => {
            // Protec 
            component.selectedTeam = team;
            let userId = users[0].id;
            component.users = users;

            // Attac
            component.removeUserFromTeam(userId, 0);

            // Eat frog for snac
            expect(mockLunchService.removeUserFromTeam).toHaveBeenCalledWith(userId, component.selectedTeam.id);
        });

        it('should remove user from users', () => {
            // Protec 
            component.selectedTeam = team;
            let userId = users[0].id;
            component.users = users;

            // Attac
            component.removeUserFromTeam(userId, 0);

            // Eat frog for snac
            expect(component.users.map(x => x.id)).not.toContain(userId);
        });
    });
});