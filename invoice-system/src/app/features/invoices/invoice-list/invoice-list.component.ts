import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceService, Invoice } from '../invoice.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './invoice-list.component.html'
})
export class InvoiceListComponent implements OnInit {

  selectedInvoiceId!: number;
  productId!: number;
  quantity!: number;
  invoices: Invoice[] = [];

  constructor(private invoiceService: InvoiceService) {}

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices() {
    this.invoiceService.getAll().subscribe(data => {
      this.invoices = data;
    });
  }

  createInvoice() {
    this.invoiceService.create().subscribe(() => {
      this.loadInvoices();
    });
  }

  addItem() {
    this.invoiceService
      .addItem(this.selectedInvoiceId, this.productId, this.quantity)
      .subscribe(() => {
        alert('Item added!');
        this.loadInvoices();
      });
  }

  closeInvoice(id: number) {
    this.invoiceService.close(id).subscribe({
      next: () => {
        alert('Invoice closed!');
        this.loadInvoices();
      },
      error: (err) => {
        alert(err.error);
      }
    });
  }
}
