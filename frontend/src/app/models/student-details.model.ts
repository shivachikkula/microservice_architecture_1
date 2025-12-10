export interface Address {
  address1: string;
  address2: string;
  city: string;
  state: string;
  zipcode: string;
}

export interface StudentDetails {
  id?: string;
  firstName: string;
  lastName: string;
  dob: string;
  gender: string;
  address1: Address;
  address2: Address;
  address3: Address;
  address4: Address;
}
