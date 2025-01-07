export interface CreateProductRequestModel {
  name: string;
  description: string;
  price: number;
  categorie: string;
  profileImage: File;
  createdOn : Date;
  isActive : boolean;
  isDeleted : boolean;
}

export interface UpdateProductRequestModel {
    id : string;
    name: string;
    description: string;
    price: number;
    categorie: string;
    profileImage: File;
    createdOn : Date;
    isActive : boolean;
    isDeleted : boolean;
  }
