import { DataTableColumnHeader } from "@/components/data-table";
import { TableDropdown } from "@/components/table-dropdown";

import { formatDate } from "@/lib/utils/date";
import { Brand } from "@/types/brand";
import { ColumnDef } from "@tanstack/react-table";

interface ColumnActions {
  onUpdate: (brand: Brand) => void;
  onSoftDelete: (brandId: string) => void;
  onHardDelete: (brandId: string) => void;
  onRestore: (brandId: string) => void;
}

export const columns = ({
  onUpdate,
  onSoftDelete,
  onHardDelete,
  onRestore,
}: ColumnActions): ColumnDef<Brand>[] => [
  {
    accessorKey: "name",
    // header: "Name",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Name" />
    ),
  },
  {
    accessorKey: "slug",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Slug" />
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
  {
    id: "actions",
    cell: ({ row }) => {
      const brand = row.original;
      return (
        <>
          <TableDropdown
            record={brand}
            actions={{
              onUpdate: () => onUpdate(brand),
              onSoftDelete: () => onSoftDelete(brand.id),
              onHardDelete: () => onHardDelete(brand.id),
              onRestore: brand.deletedAt
                ? () => onRestore(brand.id)
                : undefined,
            }}
          />
        </>
      );
    },
  },
];
