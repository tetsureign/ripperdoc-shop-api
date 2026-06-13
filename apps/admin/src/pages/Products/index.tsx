import pluralize from "pluralize";
import { useEffect } from "react";

import { ConfirmationDialog } from "@/components/confirmation-dialog";
import { DataTable } from "@/components/data-table";
import { InfoUpdateSheet } from "@/components/info-update-sheet";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { SheetTitle } from "@/components/ui/sheet";
import { Switch } from "@/components/ui/switch";
import { UI_LABELS } from "@/lib/routes";

import { ProductForm } from "./ProductForm";
import { columns } from "./productsTableColumns";
import { useProducts } from "./useProducts";
import { ScrollArea } from "@/components/ui/scroll-area";

export default function Products() {
  const { state, actions } = useProducts();
  const { table, form, delete: deleteState, error } = state;

  useEffect(() => {
    actions.fetchProducts(
      state.includeDeleted,
      state.table.pageIndex + 1,
      state.table.pageSize
    );
  }, [state.includeDeleted, state.table.pageIndex, state.table.pageSize]);

  useEffect(() => {
    table.setColumnVisibility((prev) => ({
      ...prev,
      deletedAt: state.includeDeleted,
    }));
  }, [state.includeDeleted]);

  useEffect(() => {
    if (state.product) {
      form.form.reset({
        name: state.product.name,
        description: state.product.description,
        imageUrl: state.product.imageUrl,
        price: state.product.price,
        isFeatured: state.product.isFeatured,
        categoryId: state.product.category.id,
        brandId: state.product.brand?.id ?? null,
      });
    }
  }, [state.product, form.form]);

  if (error) {
    return (
      <Alert variant="destructive">
        <AlertDescription>{error}</AlertDescription>
      </Alert>
    );
  }

  return (
    <div className="container mx-auto flex flex-col gap-4">
      <div className="flex flex-row-reverse gap-4">
        <Button
          id="create"
          onClick={actions.handleCreate}
          disabled={table.tableLoading}
        >
          Create
        </Button>
        <div className="flex items-center space-x-2">
          <Switch
            id="include-deleted"
            disabled={table.tableLoading}
            onCheckedChange={(checked) => {
              if (checked) actions.setIncludeDeleted(true);
              else actions.setIncludeDeleted(false);
            }}
          />
          <Label htmlFor="include-deleted">Include deleted</Label>
        </div>
      </div>

      <DataTable
        columns={columns({
          onUpdate: actions.handleUpdate,
          onSoftDelete: actions.handleSoftDelete,
          onHardDelete: actions.handleHardDelete,
          onRestore: actions.handleRestore,
          onFeature: actions.handleFeature,
          onUnfeature: actions.handleUnFeature,
          onViewRatings: () => {},
        })}
        data={table.data}
        loading={table.tableLoading}
        columnVisibility={table.columnVisibility}
        setColumnVisibility={table.setColumnVisibility}
        pageIndex={table.pageIndex}
        pageSize={table.pageSize}
        pageCount={table.pageCount}
        onPageChange={table.setPageIndex}
        onPageSizeChange={table.setPageSize}
      />

      <InfoUpdateSheet open={form.openSheet} onOpenChange={form.setOpenSheet}>
        <SheetTitle>
          {form.isEditMode
            ? `Update ${pluralize.singular(UI_LABELS.products)}`
            : `Create ${pluralize.singular(UI_LABELS.products)}`}
        </SheetTitle>
        <ScrollArea className="max-h-[90vh]">
          <ProductForm
            form={form.form}
            onSubmit={actions.onSubmit}
            onCancel={() => form.setOpenSheet(false)}
            isLoading={form.isDataLoading}
          />
        </ScrollArea>
      </InfoUpdateSheet>

      <ConfirmationDialog
        open={deleteState.openConfirmDialog}
        onOpenChange={deleteState.setOpenConfirmDialog}
        title={deleteState.alertMessage.title}
        description={deleteState.alertMessage.description}
        onConfirm={actions.handleConfirmDelOperation}
        isLoading={form.isDataLoading}
      />
    </div>
  );
}
