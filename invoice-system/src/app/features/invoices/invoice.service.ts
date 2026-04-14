import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface InvoiceItem {
  id: number;
  productId: number;
  quantity: number;
  invoiceId: number;
}

export interface Invoice {
  id: number;
  number: number;
  status: string;
  items: InvoiceItem[];
}


@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  private apiUrl = 'https://localhost:7094/api/invoices';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(this.apiUrl);
  }

  getItems(invoiceId: number) {
    return this.http.get<any[]>(`${this.apiUrl}/${invoiceId}/items`);
  }

  create(): Observable<Invoice> {
    return this.http.post<Invoice>(this.apiUrl, {});
  }

  addItem(invoiceId: number, request: any) {
    return this.http.post(`${this.apiUrl}/${invoiceId}/items`, request);
  }

  close(invoiceId: number) {
    return this.http.post(`${this.apiUrl}/${invoiceId}/close`, {});
  }

  removeItem(invoiceId: number, itemId: number) {
    return this.http.delete(`${this.apiUrl}/${invoiceId}/items/${itemId}`);
  }

  updateItem(invoiceId: number, itemId: number, request: any) {
    return this.http.put(`${this.apiUrl}/${invoiceId}/items/${itemId}`, request);
  }
}
