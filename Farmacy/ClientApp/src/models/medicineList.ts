namespace models {
    export interface MedicineList {
        currentPage: number,
        pagesAmount: number,
        medicines: Medicine[]
    }
}
