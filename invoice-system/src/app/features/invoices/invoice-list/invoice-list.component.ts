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
    this.invoiceService.getAll().subscribe({
      next: (data) => {
        this.invoices = data;
      },
      error: (err) => {
        console.error('Failed to load invoices', err);
      }
    });
  }

  createInvoice() {
    this.invoiceService.create().subscribe({
      next: () => {
        this.loadInvoices();
      },
      error: (err) => {
        if (err.status === 0) {
          alert('Invoice service is not working');
        } else {
          alert(err.error || 'Unexpected error');
        }
      }
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
        if (err.status === 0) {
          alert('Invoice service is not working');
        } else {
          alert(err.error || 'Unexpected error');
        }
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
        if (err.status === 0) {
          alert('Invoice service is not working');
        } else {
          alert(err.error || 'Unexpected error');
        }
      }
    });
  }



}
