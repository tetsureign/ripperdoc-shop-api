import { format } from "date-fns";

export const formatDate = (
  date: string | Date | number,
  formatStr = "dd/MM/yyyy HH:mm"
) => {
  return format(new Date(date), formatStr);
};
