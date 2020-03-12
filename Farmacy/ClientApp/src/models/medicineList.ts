import { Medicine } from './medicine';
export interface MedicineList {
    currentPage: number,
    pagesAmount: number,
    medicines: Medicine[]
}