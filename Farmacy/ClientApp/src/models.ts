export interface  Medicine {
    id: number,
    name: string;
    producer: string;
    category: string;
    form: string;
    count: number;
    component: string[];
    shelfTime: number;
}
  
export interface MedicineList {
    currentPage: number,
    pagesAmount: number,
    medicines: Medicine[]
}
  
export interface OptionSet {
    key: string,
    name: string,
    options: string[]
}

export interface ComponentSet {
    currentComponents: string[],
    availableComponents: string[]
}