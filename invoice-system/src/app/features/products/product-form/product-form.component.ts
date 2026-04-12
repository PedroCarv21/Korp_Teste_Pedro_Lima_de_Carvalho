import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateProduct, Product, ProductService } from '../product.service';
import { Input } from '@angular/core';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-form.component.html'
})
export class ProductFormComponent {

  @Input() selectedProduct: Product | null = null;
  product: CreateProduct = {
    code: '',
    description: '',
    stock: 0
  };

  constructor(private productService: ProductService) {}

  save() {
    if (this.selectedProduct) {
      const updatedProduct = {
        ...this.selectedProduct,
        ...this.product
      };

      this.productService.update(updatedProduct).subscribe(() => {
        alert('Product updated!');
        this.resetForm();
      });

    } else {
      this.productService.create(this.product).subscribe(() => {
        alert('Product created!');
        this.resetForm();
      });
    }
  }

  resetForm() {
    this.product = { code: '', description: '', stock: 0 };
    this.selectedProduct = null;
  }

  ngOnChanges() {
    if (this.selectedProduct) {
      this.product = {
        code: this.selectedProduct.code,
        description: this.selectedProduct.description,
        stock: this.selectedProduct.stock
      };
    }
  }

}
