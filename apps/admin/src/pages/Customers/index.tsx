import { useEffect } from "react";

import { DataTable } from "@/components/data-table";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";

import { columns } from "./customersTableColumns";
import { useCustomers } from "./useCustomers";

export default function Customers() {
  const { state, actions } = useCustomers();
  const { table, error } = state;

  useEffect(() => {
    actions.fetchCustomers(
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
        columns={columns}
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
    </div>
  );
}
