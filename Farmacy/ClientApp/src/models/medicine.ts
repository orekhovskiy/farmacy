namespace models{
    export interface  Medicine {
        id: number,
        name: string;
        producer: string;
        category: string;
        form: string;
        component: string[];
        shelfTime: number;
        count: number;
    }
}