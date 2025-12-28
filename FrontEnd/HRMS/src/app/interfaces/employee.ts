
export interface Employee{
  id ?: number;
  name ?: string;
  positionId ?: number;
  positionName ?: string;
  status ?: boolean;
  birthdate ?: Date;
  email ?: string;
  salary ?: number;
  departmentId ?: number;
  departmentName ?: string;
  managerId ?: number | null; // Number | Undefined | Null
  managerName ?: string | null;
  userId ?: number;
}
