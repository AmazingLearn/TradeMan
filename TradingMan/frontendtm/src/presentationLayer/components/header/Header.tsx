import React from 'react';
import { Disclosure } from '@headlessui/react'
import Navigation from "./Navigation";
import UserNavigation from "./UserNavigation";

export interface IHeaderProps {
    logout: () => void,
    user: any,
}

/**
 * Element for displaying application header used for navigation between available functionality.
 * @param props
 * @constructor
 */
function Header (props : IHeaderProps){
    return (
        <Disclosure as="nav" className="bg-gray-800">
            <div className=" mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex items-center justify-between h-16">
                    <div className="flex items-center">
                        <div className="flex-shrink-0">
                            <p className="text-white"><strong>TM</strong></p>
                        </div>
                        <Navigation/>
                    </div>
                    <UserNavigation user={props.user} Logout={props.logout}/>
                </div>
            </div>
        </Disclosure>
    );
}

export default Header;