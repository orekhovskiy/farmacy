import { Component, OnInit } from '@angular/core';
import { FilterService } from './filter.service';
import { OptionSet } from 'src/models/optionSet';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css'],
  providers: [FilterService]
})

export class FilterComponent implements OnInit {

  optionSet:OptionSet[];
  selectedOptionSet:OptionSet[];
  currentPage:number = 1;
  viewPart:string = 'all';
  isRestore:boolean;

  constructor(private filterService:FilterService) { }

  ngOnInit() {
    this.selectedOptionSet = [];
    this.filterService.getOptionSet().subscribe((data:OptionSet[]) => {
      if (data) {
        this.optionSet = data;
        
        var available: OptionSet = {
          key:'available',
          name:'Доступно',
          options:['Да', 'Нет']
        }
        this.optionSet.push(available);
        this.optionSet.forEach(element => {
          var opt:OptionSet = {
            key: element.key,
            name: element.name,
            options: []
          };
          this.selectedOptionSet.push(opt);
        });
      }
    });
  }

  setSelectedOptionSet():void {
    if (!this.isRestore) {
      this.selectedOptionSet.forEach(element => {
        element.options = [];
        $('input[name="' + element.key + '-check"]:checked').each( function () {
          element.options.push($(this).attr('value'));
        });
      });
    } else {
      this.isRestore = false;
      this.selectedOptionSet.forEach(element => {
        element.options = [];
        var storedOptions = JSON.parse(localStorage.getItem(element.key));
        storedOptions.forEach(opt => {
          element.options.push(opt);
        });
      });
    }
  }

  getFilterOptions():string {
    this.setSelectedOptionSet();
    var result:string[] = [];
    this.selectedOptionSet.forEach(element => {
      element.options.forEach(option => result.push(element.key + '=' + option));
    });
    return result.join('&');
  }

  //down chevrone - &#8964;
  //right triangle bracket - &gt;
  changeArrow(id: string) {
    var obj = document.getElementById(id);
    if (obj.innerHTML === '&gt;' )
      obj.innerHTML = '&#x2304;';
    else
      obj.innerHTML = '>';
  }
  
  storeOptions() {
    this.optionSet.forEach(opt => {
      var selected = [];
      $('input[name="' + opt.key + '-check"]:checked').each( function () {
        selected.push($(this).attr('value'));
      });
      localStorage.setItem(opt.key, JSON.stringify(selected));
    });
    localStorage.setItem('currentPage', this.currentPage.toString());
    localStorage.setItem('viewPart', this.viewPart);
    localStorage.setItem('searchKey', $('#search').val().toString());
  }

  restoreOptions() {
    this.currentPage = Number(localStorage.getItem('currentPage'));
    this.viewPart = localStorage.getItem('viewPart');
    $('#search').val(localStorage.getItem('searchKey'));
    localStorage.removeItem('currentPage');
    localStorage.removeItem('viewPart');
    localStorage.removeItem('searchKey');

    $(document).ready(function() {
      var options=['producer', 'category', 'form', 'component', 'shelfTime', 'available'];
      if (this.viewPart == 'all') {
        $(':checkbox').prop('checked', 'true');
      } else {
        options.forEach(opt => {
          var storedOptions = JSON.parse(localStorage.getItem(opt));
          storedOptions.forEach(element => {
            $(':checkbox[value="' + element + '"]').prop('checked', 'true');
          });
        });
      }
      options.forEach(opt => {localStorage.removeItem(opt)});
    });
  }

}
