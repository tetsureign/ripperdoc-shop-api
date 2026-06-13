import { Loader2 } from "lucide-react";
import { UseFormReturn } from "react-hook-form";
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
import { Textarea } from "@/components/ui/textarea";
import { UI_LABELS } from "@/lib/routes";
import pluralize from "pluralize";

export const brandFormSchema = z.object({
  name: z.string().min(1, "Name is required"),
  description: z.string(),
});

export function BrandForm({
  form,
  onSubmit,
  onCancel,
  isLoading = false,
}: {
  form: UseFormReturn<z.infer<typeof brandFormSchema>>;
  onSubmit: (values: z.infer<typeof brandFormSchema>) => void;
  onCancel: () => void;
  isLoading?: boolean;
}) {
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 mt-4">
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input
                  placeholder={`${pluralize.singular(UI_LABELS.brands)} name`}
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea
                  placeholder={`${pluralize.singular(
                    UI_LABELS.brands
                  )} description`}
                  className="resize-none"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className="flex justify-end space-x-2">
          {!isLoading ? (
            <>
              <Button variant="outline" onClick={onCancel}>
                Cancel
              </Button>
              <Button type="submit">Save changes</Button>
            </>
          ) : (
            <Button variant="outline" disabled>
              <Loader2 className="animate-spin mr-2 h-4 w-4" />
              Saving...
            </Button>
          )}
        </div>
      </form>
    </Form>
  );
}
