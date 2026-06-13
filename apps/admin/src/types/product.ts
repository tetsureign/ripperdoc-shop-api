import { Category } from "./category";
import { Brand } from "./brand";

export type Product = {
  id: string;
  name: string;
  slug: string;
  description: string;
  imageUrl: string;
  price: number;
  isFeatured: boolean;
  createdAt: Date;
  updatedAt: Date;
  deletedAt: Date | null;
  // categoryId: string;
  category: Category;
  // brandId: string | null;
  brand: Brand | null;
};
