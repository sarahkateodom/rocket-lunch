export class User {
  
    constructor() {
      this.id = undefined;
      this.name = '';
      this.nopes = [];  
    }

    public id: number;
    public name: string;
    public nopes: string[] = [];
}
