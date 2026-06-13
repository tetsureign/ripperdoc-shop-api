import { Sheet, SheetContent } from "@/components/ui/sheet";

type InfoUpdateSheetProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  children: React.ReactNode;
};

export function InfoUpdateSheet({
  open,
  onOpenChange,
  children,
}: InfoUpdateSheetProps) {
  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="container p-4">{children}</SheetContent>
    </Sheet>
  );
}
