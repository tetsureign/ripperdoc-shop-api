import { DataTableColumnHeader } from "@/components/data-table";
import { TableDropdown } from "@/components/table-dropdown";

import { formatDate } from "@/lib/utils/date";
import { Product } from "@/types/product";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";
import { ColumnDef } from "@tanstack/react-table";

interface ColumnActions {
  onUpdate: (product: Product) => void;
  onSoftDelete: (productId: string) => void;
  onHardDelete: (productId: string) => void;
  onRestore: (productId: string) => void;
  onFeature: (productId: string) => void;
  onUnfeature: (productId: string) => void;
  onViewRatings: (productId: string) => void;
}

export const columns = ({
  onUpdate,
  onSoftDelete,
  onHardDelete,
  onRestore,
  onFeature,
  onUnfeature,
  onViewRatings,
}: ColumnActions): ColumnDef<Product>[] => [
  {
    accessorKey: "imageUrl",
    header: "Image",
    cell: ({ row }) => {
      return (
        <img
          className="max-w-20"
          src={row.getValue("imageUrl")!.toString()}
        ></img>
      );
    },
  },
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
    accessorKey: "price",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Price" />
    ),
    cell: ({ row }) => {
      const amount = parseFloat(row.getValue("price"));
      const formatted = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(amount);

      return <div className="text-right font-medium">€{formatted}</div>;
    },
  },
  {
    accessorKey: "isFeatured",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Featured" />
    ),
    cell: ({ row }) =>
      row.getValue("isFeatured") === true ? <span>⭐</span> : <span>❌</span>,
  },
  {
    accessorKey: "category.name",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Category" />
    ),
  },
  {
    accessorKey: "brand.name",
    header: ({ column }) => (
      <DataTableColumnHeader column={column} title="Brand" />
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
      const product = row.original;
      return (
        <>
          <TableDropdown
            record={product}
            actions={{
              onUpdate: () => onUpdate(product),
              onSoftDelete: () => onSoftDelete(product.id),
              onHardDelete: () => onHardDelete(product.id),
              onRestore: product.deletedAt
                ? () => onRestore(product.id)
                : undefined,
            }}
          >
            <DropdownMenuItem onClick={() => onViewRatings(product.id)}>
              Ratings
            </DropdownMenuItem>

            {product.isFeatured ? (
              <DropdownMenuItem onClick={() => onUnfeature(product.id)}>
                Unfeature
              </DropdownMenuItem>
            ) : (
              <DropdownMenuItem onClick={() => onFeature(product.id)}>
                Feature
              </DropdownMenuItem>
            )}
          </TableDropdown>
        </>
      );
    },
  },
];
