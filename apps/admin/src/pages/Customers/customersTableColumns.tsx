import { DataTableColumnHeader } from "@/components/data-table";
import { formatDate } from "@/lib/utils/date";
import { Customer } from "@/types/customer";
import { ColumnDef } from "@tanstack/react-table";

export const columns: ColumnDef<Customer>[] = [
  {
    accessorKey: "userName",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Username" />
    ),
  },
  {
    accessorKey: "email",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Email" />
    ),
  },
  {
    accessorKey: "emailConfirmed",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Email Confirmed" />
    ),
  },
  {
    accessorKey: "lockoutEnabled",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Lockout Enabled" />
    ),
  },
  {
    accessorKey: "createdAt",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Created At" />
    ),
    cell: ({ getValue }) => <span>{formatDate(getValue<Date>())}</span>,
  },
  {
    accessorKey: "updatedAt",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Updated At" />
    ),
    cell: ({ getValue }) => <span>{formatDate(getValue<Date>())}</span>,
  },
  {
    accessorKey: "isDisabled",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Is Disabled" />
    ),
  },
  {
    accessorKey: "roles",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Roles" />
    ),
  },
  {
    id: "deletedAt",
    accessorKey: "deletedAt",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Deleted At" />
    ),
    cell: ({ getValue }) => {
      const value = getValue<Date | null>();
      return value ? <span>{formatDate(value)}</span> : null;
    },
  },
];
