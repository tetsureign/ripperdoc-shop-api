export type Role = ["Admin" | "Customer"];

export type Customer = {
  id: string;
  userName: string;
  email: string;
  emailConfirmed: boolean;
  lockoutEnabled: boolean;
  createdAt: Date;
  updatedAt: Date;
  deletedAt: Date | null;
  isDisabled: boolean;
  roles: Role[] | [];
};
