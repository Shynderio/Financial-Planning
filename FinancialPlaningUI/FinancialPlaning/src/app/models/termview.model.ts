export class TermViewModel {
  id: string;
  termName: string;
  creatorId: string;
  duration: number;
  startDate: Date;
  planDueDate: Date;
  reportDueDate: Date;
  status: number;
  user: any; // You might want to define a UserViewModel here if needed

  constructor(data: any = {}) {
    this.id = data.id || '';
    this.termName = data.termName || '';
    this.creatorId = data.creatorId || '';
    this.duration = data.duration || 0;
    this.startDate = new Date(data.startDate);
    this.planDueDate = new Date(data.planDueDate);
    this.reportDueDate = new Date(data.reportDueDate);
    this.status = data.status || 0;
    this.user = data.user || null;
  }
}
