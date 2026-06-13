import { Loader2 } from "lucide-react";
import pluralize from "pluralize";
import { useEffect, useState } from "react";
import { UseFormReturn } from "react-hook-form";
import { toast } from "sonner";
import z from "zod";

import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { UI_LABELS } from "@/lib/routes";
import { brandsService } from "@/services/brandsService";
import { categoriesService } from "@/services/categoriesService";
import { imageService } from "@/services/imageService";
import { Brand } from "@/types/brand";
import { Category } from "@/types/category";

export const productFormSchema = z.object({
  name: z.string().min(1, "Name is required"),
  description: z.string(),
  imageUrl: z.string().url("Invalid URL"),
  price: z.number().min(0, "Price must be a positive number"),
  isFeatured: z.boolean(),
  categoryId: z.string().uuid("Invalid category ID"),
  brandId: z
    .union([z.string().uuid("Invalid brand ID"), z.literal("__none__")])
    .nullable()
    .transform((val) => (val === "__none__" ? null : val)),
});

export function ProductForm({
  form,
  onSubmit,
  onCancel,
  isLoading = false,
}: {
  form: UseFormReturn<z.infer<typeof productFormSchema>>;
  onSubmit: (values: z.infer<typeof productFormSchema>) => void;
  onCancel: () => void;
  isLoading?: boolean;
}) {
  const [loadingFields, setLoadingFields] = useState(false);
  const [categories, setCategories] = useState<Category[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);

  const fetchCategories = async (
    includeDeleted = false,
    page = 1,
    pageSize = 50
  ) => {
    try {
      setLoadingFields(true);
      const response = await categoriesService.getAll(
        includeDeleted,
        page,
        pageSize
      );
      setCategories(response.data.categories);
    } catch (err) {
      toast.error("Unable to fetch catagories");
      console.error(err);
    } finally {
      setLoadingFields(false);
    }
  };

  const fetchBrands = async (
    includeDeleted = false,
    page = 1,
    pageSize = 50
  ) => {
    try {
      setLoadingFields(true);
      const response = await brandsService.getAll(
        includeDeleted,
        page,
        pageSize
      );
      setBrands(response.data.brands);
    } catch (err) {
      toast.error("Unable to fetch brands");
      console.error(err);
    } finally {
      setLoadingFields(false);
    }
  };

  const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files || e.target.files.length === 0) return;
    const file = e.target.files[0];

    const formData = new FormData();
    formData.append("image", file);

    setLoadingFields(true);

    try {
      const response = await imageService.upload(formData);
      form.setValue(
        "imageUrl",
        import.meta.env.VITE_API_URL + response.data.imageUrl,
        {
          shouldValidate: true,
        }
      );
    } catch (err) {
      console.error("Image upload failed", err);
      toast.error("Image upload failed. Please try again");
    } finally {
      setLoadingFields(false);
    }
  };

  useEffect(() => {
    fetchCategories();
    fetchBrands();
  }, []);

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 mt-4">
        {/* Name */}
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input
                  placeholder={`${pluralize.singular(UI_LABELS.products)} name`}
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        {/* Description */}
        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea
                  placeholder={`${pluralize.singular(
                    UI_LABELS.products
                  )} description`}
                  className="resize-none"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        {/* Image Upload */}
        <FormField
          control={form.control}
          name="imageUrl"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Image</FormLabel>
              <FormControl>
                <div className="flex items-center space-x-2">
                  <Input
                    type="text"
                    placeholder="Image URL"
                    {...field}
                    readOnly
                  />
                  <Input
                    type="file"
                    accept="image/*"
                    onChange={handleImageUpload}
                    disabled={loadingFields || isLoading}
                  />
                </div>
              </FormControl>
              {field.value && (
                <img src={field.value} alt="Product" className="mt-2 h-24" />
              )}
              <FormMessage />
            </FormItem>
          )}
        />
        {/* Price */}
        <FormField
          control={form.control}
          name="price"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Price</FormLabel>
              <FormControl>
                <div className="flex gap-2 items-center">
                  <span className="w-5">€$</span>
                  <Input
                    type="number"
                    min={0}
                    step="0.01"
                    placeholder="Price"
                    {...field}
                    onChange={(e) => field.onChange(Number(e.target.value))}
                  />
                </div>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        {/* Category ID */}
        <FormField
          control={form.control}
          name="categoryId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Category</FormLabel>
              <FormControl>
                <Select
                  value={field.value}
                  onValueChange={field.onChange}
                  onOpenChange={(open) => open && fetchCategories()}
                >
                  <SelectTrigger className="w-full">
                    {!loadingFields ? (
                      <SelectValue placeholder="Select a category" />
                    ) : (
                      <Loader2 className="animate-spin" />
                    )}
                  </SelectTrigger>
                  {!loadingFields && categories.length && (
                    <SelectContent>
                      {categories.map((item) => (
                        <SelectItem key={item.id} value={item.id}>
                          {item.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  )}
                </Select>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        {/* Brand ID */}
        <FormField
          control={form.control}
          name="brandId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Brand</FormLabel>
              <FormControl>
                <Select
                  value={field.value ?? "__none__"}
                  onValueChange={field.onChange}
                  onOpenChange={(open) => open && fetchBrands()}
                >
                  <SelectTrigger className="w-full">
                    {!loadingFields ? (
                      <SelectValue placeholder="Select a manufacturer" />
                    ) : (
                      <Loader2 className="animate-spin" />
                    )}
                  </SelectTrigger>
                  {!loadingFields && brands.length && (
                    <SelectContent>
                      <SelectItem value={"__none__"}>None</SelectItem>
                      {brands.map((item) => (
                        <SelectItem key={item.id} value={item.id}>
                          {item.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  )}
                </Select>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className="flex justify-end space-x-2">
          {!isLoading && !loadingFields ? (
            <>
              <Button variant="outline" onClick={onCancel}>
                Cancel
              </Button>
              <Button type="submit">Save changes</Button>
            </>
          ) : (
            <Button variant="outline" disabled>
              <Loader2 className="animate-spin mr-2 h-4 w-4" />
              {loadingFields ? "Loading fields..." : "Saving..."}
            </Button>
          )}
        </div>
      </form>
    </Form>
  );
}
