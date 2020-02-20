namespace models{
    export interface  Medicine {
        id: number,
        name: string;
        producer: string;
        category: string;
        form: string;
        medicineCompostion: string[];
        shelfTime: number;
        count: number;
    }
}