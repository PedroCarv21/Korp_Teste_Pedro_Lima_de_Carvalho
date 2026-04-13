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
  loading = false;
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

  addItem(form: any) {

    if (form.invalid) {
      alert('Fill all required fields!');
      return;
    }

    const request = {
      productId: this.productId,
      quantity: this.quantity
    };

    this.invoiceService.addItem(this.selectedInvoiceId, request).subscribe({
      next: () => {
        alert('Item added!');
        window.location.reload();
      },
      error: (err) => {
        const message = err.error || 'Unexpected error';
        alert(message);
      }
    });
  }

  loadingInvoiceId: number | null = null;

  closeInvoice(invoice: any) {
    if (invoice.status !== 'Open') {
      alert('Only open invoices can be printed');
      return;
    }

    this.loadingInvoiceId = invoice.id;

    this.invoiceService.close(invoice.id).subscribe({
      next: () => {
        alert('Invoice closed!');
        this.loadInvoices();
        this.loadingInvoiceId = null;
      },
      error: (err) => {
        this.loadingInvoiceId = null;
        alert(err.error);
      }
    });
  }



}
