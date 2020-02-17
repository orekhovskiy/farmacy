interface  Medicine {
    id: number,
    name: string;
    producer: string;
    category: string;
    form: string;
    count: number;
    component: string[];
    shelfTime: number;
}
  
interface MedicineList {
    currentPage: number,
    pagesAmount: number,
    medicines: Medicine[]
}
  
interface OptionSet {
    key: string,
    name: string,
    options: string[]
}