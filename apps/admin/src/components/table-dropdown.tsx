import { MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

interface TableActions {
  onUpdate: () => void;
  onSoftDelete?: () => void;
  onHardDelete?: () => void;
  onRestore?: () => void;
}

interface TableRecord {
  deletedAt: Date | null;
}

interface TableDropdownProps<T extends TableRecord> {
  record: T;
  actions: TableActions;
  children?: React.ReactNode;
}

export function TableDropdown<T extends TableRecord>({
  record,
  actions,
  children,
}: TableDropdownProps<T>) {
  const isSoftDeleted = Boolean(record.deletedAt);

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="h-8 w-8 p-0">
          <span className="sr-only">Open menu</span>
          <MoreHorizontal className="h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuLabel>Actions</DropdownMenuLabel>

        {!isSoftDeleted && (
          <>
            <DropdownMenuItem id="update" onClick={actions.onUpdate}>
              Update
            </DropdownMenuItem>

            {children}

            <DropdownMenuSeparator />
            {actions.onSoftDelete && (
              <DropdownMenuItem
                onClick={actions.onSoftDelete}
                className="text-red-600 dark:text-red-400"
              >
                Move to trash
              </DropdownMenuItem>
            )}
          </>
        )}

        {isSoftDeleted && actions.onHardDelete && (
          <DropdownMenuItem
            onClick={actions.onHardDelete}
            className="text-red-600 dark:text-red-400"
          >
            Hard delete
          </DropdownMenuItem>
        )}

        {isSoftDeleted && actions.onRestore && (
          <DropdownMenuItem onClick={actions.onRestore}>
            Restore
          </DropdownMenuItem>
        )}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
