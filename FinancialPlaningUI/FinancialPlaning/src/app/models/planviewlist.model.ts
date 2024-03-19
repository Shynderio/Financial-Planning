export class Plan {
        no: string;
        plan: string;
        term: string;
        department: string;
        status: string;
        version: number;
        user: any; // You might want to define a UserViewModel here if needed

  constructor(data: any = {}) {
    this.no = data.no || '';
    this.plan = data.plan || '';
    this.term = data.term || 0;
    this.department = data.department || '';
    this.version = data.version || 0;
    this.status = data.status || 0;
    this.user = data.user || null;
  }
}
