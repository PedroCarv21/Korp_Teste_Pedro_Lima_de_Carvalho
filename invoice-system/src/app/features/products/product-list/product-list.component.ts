import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService, Product } from '../product.service';
import { Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  @Output() productSelected = new EventEmitter<Product>();

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getAll().subscribe((data) => {
      this.products = data;
    });
  }

  editProduct(product: Product) {
    this.productSelected.emit(product);
  }

  deleteProduct(id: number) {
    if (confirm('Are you sure?')) {
      this.productService.delete(id).subscribe(() => {
        this.loadProducts();
      });
    }
  }
}
