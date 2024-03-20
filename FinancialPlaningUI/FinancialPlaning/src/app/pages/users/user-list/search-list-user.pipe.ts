import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'search',
  standalone: true
})
export class SearchListUserPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (!value) return null;
    if (!args) return value;
  
    args = args.toLowerCase();
  
    return value.filter((item: any) => {
      return  item.username.toLowerCase().includes(args);
    })
  }

}
