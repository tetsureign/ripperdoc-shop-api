export function Header({ title }: { title?: string }) {
  return (
    <header className="w-full border-b border-border bg-background px-6 py-4 shadow-sm">
      <h1 className="text-lg font-semibold tracking-tight">
        {title ?? "Ripperdoc Admin Panel"}
      </h1>
    </header>
  );
}
