import axios from 'axios';

const ADDRESS_API_BASE_URL = "/address";

class AddressService {

    getListAddress(){
        return axios.get(ADDRESS_API_BASE_URL);
    }

    getAddressById(AddressId){
        return axios.get(ADDRESS_API_BASE_URL + '/' + AddressId);
    }

    updateAddress(Address, AddressId){
        return axios.put(ADDRESS_API_BASE_URL + '/' + AddressId, Address);
    }
    
    createAddress(Address){
        return axios.post(ADDRESS_API_BASE_URL, Address);
    }

    deleteAddress(AddressId){
        return axios.delete(ADDRESS_API_BASE_URL + '/' + AddressId);
    }
}

export default new AddressService()

