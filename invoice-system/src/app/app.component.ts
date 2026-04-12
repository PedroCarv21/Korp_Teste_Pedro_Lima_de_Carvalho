import { Component, ViewChild } from '@angular/core';
import { ProductListComponent } from './features/products/product-list/product-list.component';
import { ProductFormComponent } from './features/products/product-form/product-form.component';
import { Product } from './features/products/product.service';
import { InvoiceListComponent } from './features/invoices/invoice-list/invoice-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProductListComponent, ProductFormComponent, InvoiceListComponent],
  template: `
    <app-product-form
      [selectedProduct]="selectedProduct"
      (productCreated)="onProductSaved()">
    </app-product-form>

    <hr>
    <app-invoice-list></app-invoice-list>
    <app-product-list
      (productSelected)="onProductSelected($event)">
    </app-product-list>
  `
})
export class AppComponent {

  selectedProduct: Product | null = null;

  @ViewChild(ProductListComponent)
  productList!: ProductListComponent;

  onProductSelected(product: Product) {
    this.selectedProduct = product;
  }

  onProductSaved() {
    this.productList.loadProducts();
    this.selectedProduct = null;
  }
}
