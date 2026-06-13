import { DataTableColumnHeader } from "@/components/data-table";
import { TableDropdown } from "@/components/table-dropdown";
import { formatDate } from "@/lib/utils/date";
import { Category } from "@/types/category";
import { ColumnDef } from "@tanstack/react-table";

interface ColumnActions {
  onUpdate: (category: Category) => void;
  onSoftDelete: (categoryId: string) => void;
  onHardDelete: (categoryId: string) => void;
  onRestore: (categoryId: string) => void;
}

export const columns = ({
  onUpdate,
  onSoftDelete,
  onHardDelete,
  onRestore,
}: ColumnActions): ColumnDef<Category>[] => [
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
      const category = row.original;
      return (
        <>
          <TableDropdown
            record={category}
            actions={{
              onUpdate: () => onUpdate(category),
              onSoftDelete: () => onSoftDelete(category.id),
              onHardDelete: () => onHardDelete(category.id),
              onRestore: category.deletedAt
                ? () => onRestore(category.id)
                : undefined,
            }}
          />
        </>
      );
    },
  },
];
