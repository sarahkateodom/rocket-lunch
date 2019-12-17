import { Team } from "./team";

export class User {

  constructor() {
    this.id = undefined;
    this.name = '';
    this.nopes = [];
    this.teams = [];
  }

  public id: number;
  public name: string;
  public email: string;
  public photoUrl: string;
  public nopes: string[] = [];
  public zip: string;
  public teams: Team[] = [];
}
