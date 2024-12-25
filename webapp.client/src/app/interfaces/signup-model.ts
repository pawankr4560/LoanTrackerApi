export interface SignupModel {
    firstName : string;
    lastName : string;
    email : string;
    password : string;
    confirmPassword : string;
    isActive : boolean;
    isDeleted : boolean;
    createdOn : Date;
    gender : string;
    mobile : number;
    address : string
}
