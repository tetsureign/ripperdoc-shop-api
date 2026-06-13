import { useState } from "react";

import { customersService } from "@/services/customersService";
import { Customer } from "@/types/customer";
import { VisibilityState } from "@tanstack/react-table";

export function useTableState(includeDeleted = false) {
  const [data, setData] = useState<Customer[]>([]);
  const [tableLoading, setTableLoading] = useState(true);
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({
    deletedAt: includeDeleted,
  });
  const [pageIndex, setPageIndex] = useState(0); // 0-based for TanStack Table
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [pageCount, setPageCount] = useState(0);

  return {
    data,
    setData,
    tableLoading,
    setTableLoading,
    columnVisibility,
    setColumnVisibility,
    pageIndex,
    setPageIndex,
    pageSize,
    setPageSize,
    totalCount,
    setTotalCount,
    pageCount,
    setPageCount,
  };
}

export function useCustomers() {
  const [error, setError] = useState<string | null>(null);
  const [includeDeleted, setIncludeDeleted] = useState(false);

  const tableState = useTableState(includeDeleted);

  const fetchCustomers = async (
    includeDeleted = false,
    page = 1,
    pageSize = 10
  ) => {
    try {
      tableState.setTableLoading(true);
      const response = await customersService.getAll(
        includeDeleted,
        page,
        pageSize
      );
      tableState.setData(response.data.customers);
      tableState.setTotalCount(response.data.totalCount);
      tableState.setPageCount(response.data.totalPages);
    } catch (err) {
      setError("Failed to fetch customers");
      console.error(err);
    } finally {
      tableState.setTableLoading(false);
    }
  };

  return {
    state: {
      table: tableState,
      error,
      includeDeleted,
    },
    actions: {
      setIncludeDeleted,
      fetchCustomers,
    },
  };
}
