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

  save(form: any) {

    if (form.invalid) {
      alert('Fill all required fields!');
      return;
    }

    if (this.selectedProduct) {

      const updatedProduct = {
        ...this.selectedProduct,
        ...this.product
      };

      this.productService.update(updatedProduct).subscribe({
        next: () => {
          alert('Product updated!');
          this.resetForm();
          window.location.reload();
        },
        error: (err) => {
          alert(err.error);
        }
      });

    } else {

      this.productService.create(this.product).subscribe({
        next: () => {
          alert('Product created!');
          this.resetForm();
          window.location.reload();
        },
        error: (err) => {
          if (err.status === 0) {
            alert('Product service is not working');
          } else {
            alert(err.error || 'Unexpected error');
          }
        }
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
